extends Canto
class_name Truco


func on_use() -> bool:

	if hand != null:

		is_usable = true

	if not super.on_use():

		return false

	hand.add_general_mult(int(value))

	return true
