extends Resource
class_name GameData


var seed : RandomNumberGenerator

@export var round : int


func save(new_seed : RandomNumberGenerator, new_round : int) -> void:

	seed = new_seed

	round = new_round
