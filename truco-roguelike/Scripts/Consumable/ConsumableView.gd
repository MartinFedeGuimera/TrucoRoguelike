extends TextureRect
class_name ConsumableView


@export var button : Button


func set_up(sprite : Texture2D, item_name : String) -> void:

	texture = sprite

	button.text = item_name
