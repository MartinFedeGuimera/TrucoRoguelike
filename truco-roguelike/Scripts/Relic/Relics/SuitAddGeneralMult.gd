extends RelicController
class_name SuitAddGeneralMult


@export var mult_suit : Card.CardSuit


func on_card_played(card : Card) -> void:

	if card.suit == mult_suit:

		was_used = true

		player_hand.add_general_mult(value)


func get_vars_dictionary() -> Dictionary:

	return {
		"value": value,
		"multSuit": Card.CardSuit.keys()[mult_suit]
	}
