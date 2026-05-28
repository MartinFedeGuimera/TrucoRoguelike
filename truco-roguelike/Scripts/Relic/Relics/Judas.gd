extends RelicController
class_name Judas


@export var relic_card : Card

@export var second_value : int


func on_card_played(card : Card) -> void:

	if card.name == relic_card.name:

		card.mult *= value

		player_hand.add_general_mult(
			-second_value
		)


func get_vars_dictionary() -> Dictionary:

	return {
		"value": value,
		"relicCard": relic_card.name,
		"secondValue": second_value
	}
