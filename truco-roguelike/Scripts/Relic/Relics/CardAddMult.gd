extends RelicController
class_name CardAddMult


@export var relic_card : Card


func on_card_played(card : Card) -> void:

	if card.name == relic_card.name:

		card.mult += value

	super.on_card_played(card)


func get_vars_dictionary() -> Dictionary:

	return {
		"value": value,
		"relicCard": relic_card.name
	}
