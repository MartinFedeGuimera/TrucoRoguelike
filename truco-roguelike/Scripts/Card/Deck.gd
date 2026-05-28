extends Resource
class_name Deck


@export var deck_data : Array[Card] = []


var cards : Array[Card] = []


func shuffle(seed : RandomNumberGenerator) -> void:

	cards.clear()

	for card in deck_data:

		cards.append(card.duplicate(true))

	for i in range(cards.size() - 1, 0, -1):

		var random_index = seed.randi_range(0, i)

		var temp = cards[i]

		cards[i] = cards[random_index]

		cards[random_index] = temp


func get_cards() -> Array[Card]:
	return cards


func remove_at(index : int) -> void:

	cards.remove_at(index)
