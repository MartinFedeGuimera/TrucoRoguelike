extends Node
class_name Hand

@export_group("Sounds")
@export var draw_card_sound : AudioStream
@export var play_card_sound : AudioStream

var sfx_player : AudioStreamPlayer2D


@export_group("Objects")
@export var game_controller : GameController
@export var enemy : Enemy
@export var dmg_ui_controller : DmgUiController

var player : PlayerController
var seed : RandomNumberGenerator


@export_group("Data")
@export var card_scene : PackedScene
@export var deck_resource : Deck

var max_cards_drawn : int = 3

var drawn_cards : Array[CardController] = []

var selected_card : CardController

var can_start : bool = true

var temp_mult : float = 0.0
var general_mult : float = 0.0
var permanent_mult : float = 0.0
var damage_multiplier : float = 1.0

var has_flor : bool = false


signal card_selected(card)
signal attack(damage)
signal out_of_cards


func create_external_attack(damage : int, mult : float = 1.0, extra_temp_mult : float = 0.0) -> void:
	var attack_data = {
		"damage": damage,
		"card_mult": mult,
		"general_mult": general_mult,
		"temp_mult": temp_mult + extra_temp_mult,
		"damage_multiplier": damage_multiplier,
		"card": null
	}

	damage_multiplier = 1.0

	wait_for_damage_animations(attack_data)


func _ready() -> void:
	player = get_parent()

	sfx_player = get_node("SfxPlayer")

	seed = game_controller.get_seed()

	deck_resource.shuffle(seed)

	permanent_mult = PlayerData.instance.permanent_mult


func _process(delta : float) -> void:
	if !can_start:
		return

	can_start = false

	general_mult = permanent_mult

	dmg_ui_controller.reset_round()

	if PlayerData.instance.relics != null:
		for relic in PlayerData.instance.relics:
			relic.set_up(self)
			relic.on_player_turn_started()

			dmg_ui_controller.apply_perk_mult(get_current_mult())

	draw_cards()


func draw_cards() -> void:
	for i in range(max_cards_drawn):

		if deck_resource.get_cards().size() == 0:
			return

		var new_data : Card = deck_resource.get_cards()[0]

		var new_card : CardController = card_scene.instantiate()

		new_card.set_up(new_data, self)

		drawn_cards.append(new_card)

		add_child(new_card)

		new_card.position = Vector2(600 + 100 * (i + 1), 700)

		deck_resource.remove_at(0)

		has_flor = check_flor()

		sfx_player.stream = draw_card_sound
		sfx_player.pitch_scale = seed.randf_range(0.8, 1.1)
		sfx_player.play()

		await get_tree().create_timer(0.15).timeout


func on_card_played() -> void:

	if selected_card == null:
		return

	temp_mult = 0

	var played_card : CardController = selected_card

	var card_data : Card = played_card.get_data()

	if PlayerData.instance.relics != null:

		for relic in PlayerData.instance.relics:

			relic.set_up(self)

			relic.on_card_played(card_data)

			dmg_ui_controller.apply_perk_mult(get_current_mult())

	var attack_data = {
		"damage": card_data.value,
		"card_mult": card_data.mult,
		"general_mult": general_mult,
		"temp_mult": temp_mult,
		"damage_multiplier": damage_multiplier,
		"card": played_card
	}

	damage_multiplier = 1.0

	drawn_cards.erase(played_card)

	played_card.queue_free()

	selected_card = null

	wait_for_damage_animations(attack_data)

	if drawn_cards.size() == 0:

		if PlayerData.instance.relics != null:

			for relic in PlayerData.instance.relics:
				relic.on_player_turn_finished()

		emit_signal("out_of_cards")


func wait_for_damage_animations(attack_data : Dictionary) -> void:

	sfx_player.stream = play_card_sound

	sfx_player.pitch_scale = seed.randf_range(0.8, 1.1)

	sfx_player.play()

	await get_tree().create_timer(0.3).timeout

	deal_damage(attack_data)


func deal_damage(attack_data : Dictionary) -> void:

	var final_damage = attack_data.damage * (
		attack_data.card_mult +
		attack_data.general_mult +
		attack_data.temp_mult
	)

	final_damage *= attack_data.damage_multiplier

	emit_signal("attack", int(final_damage))


func check_flor() -> bool:

	if drawn_cards.size() == 0:
		return false

	var first_suit = drawn_cards[0].get_data().suit

	for i in range(1, drawn_cards.size()):

		if drawn_cards[i].get_data().suit != first_suit:
			return false

	return true


func set_selected_card(card : CardController) -> void:

	selected_card = card

	dmg_ui_controller.update_from_card(
		selected_card,
		general_mult,
		temp_mult
	)

	emit_signal("card_selected", selected_card)


func add_temp_mult(added_mult : int) -> void:
	temp_mult += added_mult


func add_general_mult(added_mult : int) -> void:
	general_mult += added_mult


func add_perma_mult(added_mult : int) -> void:

	permanent_mult = max(0, permanent_mult + added_mult)

	general_mult = max(0, general_mult + added_mult)

	PlayerData.instance.permanent_mult = permanent_mult


func add_damage_multiplier(mult : float) -> void:
	damage_multiplier *= mult


func get_drawn_cards() -> Array[Card]:

	var cards_data : Array[Card] = []

	for controller in drawn_cards:
		cards_data.append(controller.get_data())

	return cards_data


func get_current_mult() -> float:

	if selected_card == null:
		return general_mult + temp_mult

	var data : Card = selected_card.get_data()

	return data.mult + general_mult + temp_mult
