extends RelicController
class_name BilleteraMataGalan


func on_player_turn_started() -> void:

	player_hand.add_general_mult(
		PlayerData.instance.money
	)
