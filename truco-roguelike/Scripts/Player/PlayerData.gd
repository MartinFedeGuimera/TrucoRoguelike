extends Node

signal data_changed

static var instance : PlayerData

var money : int = 0

var relics : Array[RelicController] = []
var max_relics : int = 4

var consumables : Array[Consumable] = []
var max_consumables : int = 2

var max_health : int = 50
var health : int

var permanent_mult : float = 0.0


func _ready() -> void:
	instance = self
	health = max_health


func add_money(added_money : int) -> void:
	money += added_money
	emit_signal("data_changed")


func add_perma_mult(added_perma_mult : float) -> void:
	permanent_mult += added_perma_mult


func add_health(added_health : int) -> void:
	health = clamp(health + added_health, 0, max_health)


func remove_relic(relic_name : String) -> void:
	for i in range(relics.size()):
		if relic_name == relics[i].name:
			relics.remove_at(i)
			break

	emit_signal("data_changed")


func remove_consumable(consumable_name : String) -> void:
	for i in range(consumables.size()):
		if consumable_name == consumables[i].name:
			consumables.remove_at(i)
			break

	emit_signal("data_changed")
