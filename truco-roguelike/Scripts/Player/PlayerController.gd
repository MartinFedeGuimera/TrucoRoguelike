extends Node
class_name PlayerController


@export var game_controller : GameController

@export var enemy : Enemy

@export var description_controller : DescriptionController


var hand : Hand


signal turn_ended


@export_group("Scenes")
@export var relic_scene : PackedScene

@export var consumable_scene : PackedScene


@export_group("UI")
@export var relics_parent : Node

@export var consumables_parent : Node


func _ready() -> void:

	hand = get_node("Hand")

	description_controller.set_up()

	hand.attack.connect(deal_damage)

	hand.out_of_cards.connect(on_out_of_cards)

	enemy.turn_ended.connect(start_turn)

	game_controller.data_loaded.connect(load_data)

	update_ui()


func _process(delta : float) -> void:

	if PlayerData.instance.health <= 0:

		print("Game Over!")


# ---------------- DAMAGE ----------------


func deal_damage(damage : int) -> void:

	enemy.take_damage(damage)

	print("Damage Dealed: " + str(damage))


func take_damage(damage : int) -> void:

	PlayerData.instance.health -= damage


# ---------------- TURN ----------------


func start_turn() -> void:

	hand.can_start = true

	print("Health: " + str(PlayerData.instance.health))


# ---------------- MONEY ----------------


func add_money(added_money : int) -> void:

	PlayerData.instance.money += added_money


# ---------------- UI ----------------


func update_ui() -> void:

	for child in relics_parent.get_children():

		child.queue_free()

	for relic in PlayerData.instance.relics:

		var relic_node : RelicView = relic_scene.instantiate()

		relic_node.set_up(relic, description_controller)

		relics_parent.add_child(relic_node)

	for child in consumables_parent.get_children():

		child.queue_free()

	for consumable in PlayerData.instance.consumables:

		var controller : ConsumableController = consumable_scene.instantiate()

		controller.set_up(
			consumable,
			hand,
			description_controller
		)

		consumables_parent.add_child(controller)


# ---------------- LOAD ----------------


func load_data() -> void:

	print("Player Data Loaded")

	update_ui()


func on_out_of_cards() -> void:

	emit_signal("turn_ended")
