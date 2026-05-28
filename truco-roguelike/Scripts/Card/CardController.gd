extends Node2D
class_name CardController


var data : Card

var sprite : Sprite2D


@export var hoover_mult : float = 1.2

@export var card_size : Vector2 = Vector2(5, 5)


var is_selected : bool = false

var player_hand : Hand


func _ready() -> void:

	sprite = get_node("Sprite2D")

	player_hand.card_selected.connect(on_card_selected)


func on_card_selected(card_controller : CardController) -> void:

	var card_data : Card = card_controller.get_data()

	if card_data.name != data.name:

		scale = card_size

		is_selected = false


func _on_area_2d_input_event(
	viewport,
	event : InputEvent,
	shape_idx
) -> void:

	if event is InputEventMouseButton and event.pressed:

		is_selected = true

		player_hand.set_selected_card(self)

		scale = card_size * hoover_mult


func _on_area_2d_mouse_entered() -> void:

	if !is_selected:

		scale = card_size * hoover_mult


func _on_area_2d_mouse_exited() -> void:

	if !is_selected:

		scale = card_size


func set_up(new_data : Card, hand : Hand) -> void:

	data = new_data

	player_hand = hand

	sprite = get_node("Sprite2D")

	sprite.texture = data.texture

	scale = card_size


func get_data() -> Card:
	return data


func _exit_tree() -> void:

	if player_hand != null:
		player_hand.card_selected.disconnect(on_card_selected)
