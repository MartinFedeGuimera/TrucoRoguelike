extends Node
class_name ConsumableController


var view : ConsumableView

var data : Consumable

var sell_button : Button

var description_controller : DescriptionController


func set_up(
	new_data : Consumable,
	hand : Hand,
	new_description_controller : DescriptionController
) -> void:

	print("Set Up called")

	view = get_node("TextureRect")

	sell_button = get_node("SellButton")

	view.set_up(
		new_data.sprite,
		new_data.name
	)

	new_data.hand = hand

	data = new_data

	description_controller = new_description_controller

	data.consumable_used.connect(on_used)


func set_up_shop(
	new_data : Consumable,
	new_description_controller : DescriptionController
) -> void:

	print("Set Up called")

	view = get_node("TextureRect")

	sell_button = get_node("SellButton")

	view.set_up(
		new_data.sprite,
		new_data.name
	)

	new_data.is_usable = false

	data = new_data

	description_controller = new_description_controller

	data.consumable_used.connect(on_used)


func get_data() -> Consumable:
	return data


func on_button_pressed() -> void:

	print("Consumable used")

	data.on_use()


func on_used() -> void:

	print("Consumable Destroyed")

	PlayerData.instance.remove_consumable(data.name)

	queue_free()


func on_sell() -> void:

	PlayerData.instance.add_money(data.price / 2)

	PlayerData.instance.remove_consumable(data.name)

	queue_free()


func _on_mouse_entered() -> void:

	sell_button.visible = true

	description_controller.change_data(
		data.name,
		data.description,
		data.get_vars_dictionary()
	)

	description_controller.on_show()


func _on_mouse_exited() -> void:

	sell_button.visible = false

	description_controller.on_hide()


func _exit_tree() -> void:

	if data != null:

		if data.consumable_used.is_connected(on_used):

			data.consumable_used.disconnect(on_used)
