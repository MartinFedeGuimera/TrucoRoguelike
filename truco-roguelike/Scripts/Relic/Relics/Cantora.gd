extends RelicController
class_name Cantora


var suit : Card.CardSuit = Card.CardSuit.BASTO


@export var substract_value : int


func on_player_turn_started() -> void:

	var rng := RandomNumberGenerator.new()

	rng.randomize()

	while true:

		suit = rng.randi_range(
			0,
			Card.CardSuit.size() - 1
		)

		if suit != Card.CardSuit.NONE:

			break

	print("Suit Selected: ", suit)
