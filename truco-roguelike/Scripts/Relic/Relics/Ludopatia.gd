extends RelicController
class_name Ludopatia


var rng := RandomNumberGenerator.new()

var random_mult : int


func on_card_played(card : Card) -> void:

	rng.randomize()

	var biased_random := pow(rng.randf(), 2.5)

	random_mult = int(round(biased_random * value))

	print("Random Mult: ", random_mult)

	player_hand.add_temp_mult(random_mult)
