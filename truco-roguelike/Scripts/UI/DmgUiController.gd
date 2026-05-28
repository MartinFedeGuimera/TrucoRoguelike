extends Node
class_name DmgUiController


@export var game_controller : GameController

@export var mult_sound : AudioStream

var sfx_player : AudioStreamPlayer2D


var attack_label : Label

var mult_label : Label


var displayed_attack : int = 0

var displayed_mult : float = 0.0


var last_mult : float = 0.0


var mult_sound_pitch : float = 0.8

var last_played_mult : int = 0


var mult_queue : Array[float] = []


var processing_queue : bool = false


func _ready() -> void:

	attack_label = get_node("AttackLabel")

	mult_label = get_node("MultLabel")

	sfx_player = get_node("SfxPlayer")

	displayed_attack = 0

	displayed_mult = 0


func play_mult_sound() -> void:

	if mult_sound == null:
		return

	sfx_player.stream = mult_sound

	sfx_player.pitch_scale = mult_sound_pitch

	mult_sound_pitch += 0.05

	sfx_player.play()


func process_queue() -> void:

	if processing_queue:
		return

	processing_queue = true

	var delay : float = 0.2

	while mult_queue.size() > 0:

		var next_mult = mult_queue.pop_front()

		displayed_mult = next_mult

		update_labels()

		play_mult_sound()

		last_mult = displayed_mult

		await get_tree().create_timer(delay).timeout

		delay *= 0.8

		delay = max(0.03, delay)

	mult_sound_pitch = 0.8

	processing_queue = false


func update_from_card(
	card : CardController,
	general_mult : float,
	temp_mult : float
) -> void:

	mult_queue.clear()

	mult_sound_pitch = 0.8

	var attack : int = 0

	var mult : float = general_mult + temp_mult

	if card != null:

		var data : Card = card.get_data()

		attack = data.value

		mult += data.mult

	displayed_attack = attack

	displayed_mult = mult

	last_mult = mult

	update_labels()


func apply_perk_mult(new_mult : float) -> void:

	if new_mult <= last_mult:
		return

	mult_queue.append(new_mult)

	process_queue()


func update_labels() -> void:

	attack_label.text = "Attack: " + str(displayed_attack)

	mult_label.text = "Mult: " + str(displayed_mult)


func reset_round() -> void:

	mult_queue.clear()

	processing_queue = false

	displayed_attack = 0

	displayed_mult = 0

	last_mult = 0

	mult_sound_pitch = 0.8

	update_labels()
