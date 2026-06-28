using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class DescriptionFormatter
{
    private static Dictionary<string, string> keywordColors = new Dictionary<string, string>()
    {
        { "Mult", "red" },
        { "General Mult", "red" },
        { "Temp Mult", "red" },
        { "Perma Mult", "red" },
        { "Card Mult", "red" },
        { "Enemy", "darkRed" },
        { "Health", "green" },
        { "Damage", "cyan" },
        { "Envido", "cyan" },
        { "Basto", "darkGreen" },
        { "Oro", "yellow" },
        { "Espada", "lightBlue" },
        { "Copa", "fd0041" }
    };

    public static string Format(string template, Dictionary<string, object> variables)
    {
        string result = template;

        foreach (var pair in variables)
        {
            result = result.Replace(
                "{" + pair.Key + "}",
                ConvertValue(pair.Value)
            );
        }

        result = Regex.Replace(result,
            @"\{([^|]+)\|([^}]+)\}",
            match =>
            {
                int min = ResolveNumber(match.Groups[1].Value, variables);
                int max = ResolveNumber(match.Groups[2].Value, variables);

                int value = (int)GD.RandRange(min, max);
                return value.ToString();
            });

        result = Regex.Replace(
            result,
            @"\$\d+",
            match => $"[color=yellow]{match.Value}[/color]"
        );

        foreach (var keyword in keywordColors)
        {
            string suitPattern = $@"\b({keyword.Key})_(\d+)\b";

            result = Regex.Replace(
                result,
                suitPattern,
                match =>
                {
                    string suit = match.Groups[1].Value;
                    string number = match.Groups[2].Value;

                    return $"[color={keyword.Value}]{suit} {number}[/color]";
                },
                RegexOptions.IgnoreCase
            );
        }

        foreach (var keyword in keywordColors)
        {
            string pattern =
                $@"(([+-]\s*\d+|[xX×]\s*\d+)\s*{keyword.Key})";

            result = Regex.Replace(
                result,
                pattern,
                match => $"[color={keyword.Value}]{match.Value}[/color]",
                RegexOptions.IgnoreCase
            );
        }

        foreach (var keyword in keywordColors)
        {
            string pattern = $@"\b({Regex.Escape(keyword.Key)})\s+([+-]?\d+)\b";

            result = Regex.Replace(
                result,
                pattern,
                match =>
                {
                    return $"[color={keyword.Value}]{match.Groups[1].Value} {match.Groups[2].Value}[/color]";
                },
                RegexOptions.IgnoreCase
            );
        }

        foreach (var keyword in keywordColors)
        {
            result = Regex.Replace(
                result,
                $@"\b{keyword.Key}\b",
                $"[color={keyword.Value}]{keyword.Key}[/color]",
                RegexOptions.IgnoreCase
            );
        }

        return result;
    }

    private static string ConvertValue(object value)
    {
        if (value == null)
            return "";

        switch (value)
        {
            case int i:
                return i.ToString();

            case float f:
                return f.ToString("0.##");

            case string s:
                return s;

            case bool b:
                return b ? "Yes" : "No";

            default:
                return value.ToString();
        }
    }

    private static int ResolveNumber(
        string input,
        Dictionary<string, object> variables)
    {
        input = input.Trim();

        if (int.TryParse(input, out int number))
            return number;

        if (variables.ContainsKey(input))
            return Convert.ToInt32(variables[input]);

        return 0;
    }
}