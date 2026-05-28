extends Node
class_name DescriptionFormatter


static var keyword_colors := {
	"mult": "red",
	"general mult": "red",
	"temp mult": "red",
	"perma mult": "red",
	"card mult": "red",
	"enemy": "darkRed",
	"health": "green",
	"damage": "cyan",
	"Basto": "darkGreen",
	"Oro": "yellow",
	"Espada": "lightBlue",
	"Copa": "fd0041"
}


static func format(template : String, variables : Dictionary) -> String:

	var result = template


	for key in variables.keys():

		result = result.replace(
			"{" + str(key) + "}",
			convert_value(variables[key])
		)


	var range_regex = RegEx.new()

	range_regex.compile("\\{([^|]+)\\|([^}]+)\\}")


	for match in range_regex.search_all(result):

		var min_value = resolve_number(
			match.get_string(1),
			variables
		)

		var max_value = resolve_number(
			match.get_string(2),
			variables
		)

		var value = randi_range(min_value, max_value)

		result = result.replace(
			match.get_string(),
			str(value)
		)


	var money_regex = RegEx.new()

	money_regex.compile("\\$\\d+")


	for match in money_regex.search_all(result):

		var colored = (
			"[color=yellow]" +
			match.get_string() +
			"[/color]"
		)

		result = result.replace(
			match.get_string(),
			colored
		)


	for keyword in keyword_colors.keys():

		var suit_regex = RegEx.new()

		suit_regex.compile(
			"\\b(" + keyword + ")_(\\d+)\\b"
		)

		for match in suit_regex.search_all(result):

			var suit = match.get_string(1)

			var number = match.get_string(2)

			var colored = (
				"[color=" +
				keyword_colors[keyword] +
				"]" +
				suit +
				" " +
				number +
				"[/color]"
			)

			result = result.replace(
				match.get_string(),
				colored
			)


	for keyword in keyword_colors.keys():

		var stat_regex = RegEx.new()

		stat_regex.compile(
			"(([+-]\\s*\\d+|[xX×]\\s*\\d+)\\s*" +
			keyword +
			")"
		)

		for match in stat_regex.search_all(result):

			var colored = (
				"[color=" +
				keyword_colors[keyword] +
				"]" +
				match.get_string() +
				"[/color]"
			)

			result = result.replace(
				match.get_string(),
				colored
			)


	for keyword in keyword_colors.keys():

		var keyword_regex = RegEx.new()

		keyword_regex.compile("\\b" + keyword + "\\b")

		for match in keyword_regex.search_all(result):

			var colored = (
				"[color=" +
				keyword_colors[keyword] +
				"]" +
				match.get_string() +
				"[/color]"
			)

			result = result.replace(
				match.get_string(),
				colored
			)


	return result


static func convert_value(value) -> String:

	if value == null:
		return ""

	if value is int:
		return str(value)

	if value is float:
		return str(snapped(value, 0.01))

	if value is String:
		return value

	if value is bool:
		return "Yes" if value else "No"

	return str(value)


static func resolve_number(
	input : String,
	variables : Dictionary
) -> int:

	input = input.strip_edges()

	if input.is_valid_int():
		return int(input)

	if variables.has(input):
		return int(variables[input])

	return 0
