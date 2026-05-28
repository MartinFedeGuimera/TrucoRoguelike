extends RelicController
class_name CardAddGeneralMult


@export var relic_card : Card


func on_card_played(card : Card) -> void:

	if card.name == relic_card.name:

		was_used = true

		player_hand.add_general_mult(value)


func get_vars_dictionary() -> Dictionary:

	return {
		"value": value,
		"relicCard": relic_card.name
	}
