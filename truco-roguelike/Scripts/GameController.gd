extends Node2D
class_name GameController


@export var player : PlayerController

@export var player_hand : Hand

@export var enemy : Enemy

@export var game_data : GameData

@export var round_win_prize : int


var round : int = 0

var game_loaded : bool = false


var seed : RandomNumberGenerator = RandomNumberGenerator.new()


signal card_selected(card)

signal loading_scene

signal data_loaded


func _ready() -> void:

	if game_data.round != 0:
		load_data()

	round += 1

	print("Current Round: " + str(round))

	if game_data.round == 1:
		seed.randomize()

	player_hand.card_selected.connect(on_card_selected)

	enemy.enemy_dead.connect(on_enemy_killed)

	print("Seed: " + str(seed.randi()))


func on_card_selected(card : CardController) -> void:

	if is_instance_valid(card):

		emit_signal("card_selected", card)


func on_enemy_killed() -> void:

	player.add_money(round_win_prize)

	round_win_prize = int(round_win_prize * 1.5)

	load_shop()


func load_shop() -> void:

	emit_signal("loading_scene")

	save_data()

	SceneManager.load("res://Scenes/Screens/shop.tscn")


func save_data() -> void:

	game_data.save(seed, round)

	print("Game Data Saved")


func load_data() -> void:

	seed = game_data.seed

	round = game_data.round

	print("Game Data Loaded")

	emit_signal("data_loaded")


func get_seed() -> RandomNumberGenerator:
	return seed


func get_round() -> int:
	return round


func is_game_loaded() -> bool:
	return game_loaded
