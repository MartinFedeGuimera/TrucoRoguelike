extends Consumable
class_name Guiso


func on_use() -> bool:

	if PlayerData.instance.health == PlayerData.instance.max_health:

		is_usable = false

	else:

		is_usable = true

	if not super.on_use():

		return false

	PlayerData.instance.add_health(int(value))

	return true
