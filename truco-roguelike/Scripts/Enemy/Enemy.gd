extends Node
class_name Enemy


@export var game_controller : GameController

@export var player : PlayerController

@export var max_health : int

var health : int

@export var damage : int


signal turn_ended

signal enemy_dead


var is_dead : bool = false


func _ready() -> void:
	print("GameController:", game_controller)

	health = max_health

	player.turn_ended.connect(deal_damage)

	game_controller.data_loaded.connect(calculate_max_health)


func _process(delta : float) -> void:

	if health <= 0 and !is_dead:

		is_dead = true

		print("Enemy Killed")

		on_death()


func deal_damage() -> void:

	if !is_dead:

		player.take_damage(damage)

		print("Damage Taken: " + str(damage))

		emit_signal("turn_ended")


func take_damage(added_damage : int) -> void:

	health -= added_damage

	print("Enemy Health: " + str(health))


func on_death() -> void:

	emit_signal("enemy_dead")


func calculate_max_health() -> void:

	max_health *= game_controller.get_round()

	health = max_health


func get_health() -> int:
	return health


func get_max_health() -> int:
	return max_health
