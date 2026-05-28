extends Resource
class_name RelicController

@export var name : String
@export_multiline var description : String
@export var sprite_texture : Texture2D
@export var value : int
@export var price : int = 5

var was_used : bool = false

var player_hand : Hand


func set_up(hand : Hand) -> void:
	player_hand = hand
	was_used = false


func get_vars_dictionary() -> Dictionary:
	return {
		"value": value
	}


func on_card_played(card : Card) -> void:
	was_used = true


func on_player_turn_started() -> void:
	was_used = true


func on_player_turn_finished() -> void:
	was_used = true


func on_enemy_attacks() -> void:
	was_used = true
