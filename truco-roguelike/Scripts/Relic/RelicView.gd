extends Node
class_name RelicView


var texture_rect : TextureRect

var sell_button : Button

var data : RelicController

var description_controller : DescriptionController


func set_up(
	new_data : RelicController,
	new_description_controller : DescriptionController
) -> void:

	texture_rect = get_node("TextureRect")

	sell_button = get_node("SellButton")

	data = new_data

	texture_rect.texture = data.sprite_texture

	description_controller = new_description_controller


func on_sell() -> void:

	PlayerData.instance.add_money(data.price / 2)

	print("Relic Sold")

	PlayerData.instance.remove_relic(data.name)

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
