extends RelicController
class_name ACaraDePerro


var can_use : bool = false

var previous_health : int


@export var substract_value : int


func on_player_turn_started() -> void:

	super.on_player_turn_started()

	if can_use:

		if PlayerData.instance.health > previous_health:

			player_hand.add_perma_mult(
				-substract_value
			)

		else:

			player_hand.add_perma_mult(value)


func on_player_turn_finished() -> void:

	super.on_player_turn_finished()

	can_use = true

	previous_health = PlayerData.instance.health


func get_vars_dictionary() -> Dictionary:

	return {
		"value": value,
		"substractValue": substract_value
	}
