extends RelicController
class_name CardMultMult


@export var relic_cards : Array[Card]


func on_card_played(card : Card) -> void:

	for relic_card in relic_cards:

		if relic_card.name == card.name:

			card.mult *= value

			break

	super.on_card_played(card)
