extends Control
class_name RelicProduct


var shop : ShopController


var texture_rect : TextureRect

var price_label : Label


var relic_data : RelicController


var description_controller : DescriptionController


func set_up(
	new_relic_data : RelicController,
	new_shop : ShopController,
	new_description_controller : DescriptionController
) -> void:

	texture_rect = get_node("TextureRect")

	price_label = get_node("PriceLabel")

	texture_rect.texture = new_relic_data.sprite_texture

	price_label.text = "$" + str(new_relic_data.price)

	relic_data = new_relic_data

	description_controller = new_description_controller

	shop = new_shop


func _on_mouse_entered() -> void:

	description_controller.change_data(
		relic_data.name,
		relic_data.description,
		relic_data.get_vars_dictionary()
	)

	description_controller.on_show()


func _on_mouse_exited() -> void:

	description_controller.on_hide()


func on_try_buy() -> void:

	print("Trying Buy Relic: " + relic_data.name)

	shop.try_buy_relic(relic_data)


func get_data() -> RelicController:
	return relic_data
