extends Consumable
class_name Canto


@export var value_upgrade : int
@export var max_level : int

var level : int = 0


func on_level_up() -> void:

	if level + 1 <= max_level:

		level += 1
		value += value_upgrade

		print(name, " Level Up! Current Level: ", level)
		print("Value Added: ", value_upgrade)


func on_use() -> bool:

	if not super.on_use():

		return false

	on_level_up()

	return true


func get_vars_dictionary() -> Dictionary:

	return {
		"value": value,
		"valueUpgrade": value_upgrade,
		"level": level
	}
