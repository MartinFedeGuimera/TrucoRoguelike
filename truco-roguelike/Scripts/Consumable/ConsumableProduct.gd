extends Control
class_name ConsumableProduct


var shop : ShopController


var name_label : Label

var price_label : Label


var consumable_data : Consumable


var description_controller : DescriptionController


func set_up(
	consumable : Consumable,
	new_shop : ShopController,
	new_description_controller : DescriptionController
) -> void:

	name_label = get_node("NameLabel")

	price_label = get_node("PriceLabel")

	name_label.text = consumable.name

	price_label.text = "$" + str(consumable.price)

	consumable_data = consumable

	description_controller = new_description_controller

	shop = new_shop


func _on_mouse_entered() -> void:

	description_controller.change_data(
		consumable_data.name,
		consumable_data.description,
		consumable_data.get_vars_dictionary()
	)

	description_controller.on_show()


func _on_mouse_exited() -> void:

	description_controller.on_hide()


func on_try_buy() -> void:

	print(
		"Trying Buy Consumable: " +
		consumable_data.name
	)

	shop.try_buy_consumable(consumable_data)


func get_data() -> Consumable:
	return consumable_data
