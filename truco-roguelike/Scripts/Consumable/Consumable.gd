extends Resource
class_name Consumable

signal consumable_used

var is_usable : bool = true

@export var name : String
@export_multiline var description : String
@export var sprite : Texture2D
@export var price : int
@export var value : int

var hand : Hand


func on_use() -> bool:
	if !is_usable:
		print("Consumable can't be used")
		return false

	if hand == null:
		print("Hand is NULL")
		return false

	print("Consumable Used: " + name)

	emit_signal("consumable_used")

	return true


func get_vars_dictionary() -> Dictionary:
	return {
		"value": value
	}
