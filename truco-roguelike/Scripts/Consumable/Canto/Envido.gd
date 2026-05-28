extends Canto
class_name Envido


var envido_value : int = 0


func on_use() -> bool:

	if hand != null:

		is_usable = true

	if not super.on_use():

		return false

	envido_value = calculate_envido_value(hand.get_drawn_cards())

	hand.create_external_attack(
		int(value) + envido_value
	)

	return true


func calculate_envido_value(hand_cards : Array) -> int:

	if hand_cards.size() == 0:

		return 0


	var suits := {}


	for card in hand_cards:

		if not suits.has(card.suit):

			suits[card.suit] = []


		suits[card.suit].append(card.envido_value)


	var best_pair_value := -1
	var highest_single := 0


	for suit in suits.keys():

		var values : Array = suits[suit]

		values.sort()
		values.reverse()

		highest_single = max(highest_single, values[0])

		if values.size() >= 2:

			var pair_value = values[0] + values[1] + 20

			best_pair_value = max(best_pair_value, pair_value)


	if best_pair_value >= 0:

		return best_pair_value


	return highest_single
