extends Control
class_name DescriptionController


var title : Label

var description : RichTextLabel

var margin : MarginContainer


var is_visible_description : bool = false


@export var max_description_width : float = 300.0

func _ready():
	if not mouse_exited.is_connected(on_hide):
		mouse_exited.connect(on_hide)
	if not mouse_entered.is_connected(on_hide):
		mouse_entered.connect(on_hide)


func set_up() -> void:

	set_anchors_preset(Control.PRESET_TOP_LEFT)

	title = get_node("MarginContainer/VBoxContainer/Title")

	description = get_node(
		"MarginContainer/VBoxContainer/Description"
	)

	margin = get_node("MarginContainer")

	description.autowrap_mode = TextServer.AUTOWRAP_WORD

	description.custom_minimum_size = Vector2(
		max_description_width,
		0
	)


func change_data(
	title_text : String,
	description_text : String,
	vars : Dictionary
) -> void:

	title.text = title_text

	var final_description = DescriptionFormatter.format(
		description_text,
		vars
	)

	description.text = final_description

	print(description.text)

	await get_tree().process_frame

	reset_size()


func _process(delta : float) -> void:

	if !is_visible_description:
		return

	var mouse_pos = get_viewport().get_mouse_position()

	var viewport_size = get_viewport_rect().size

	var pos = mouse_pos + Vector2(16, 16)

	if pos.x + margin.size.x > viewport_size.x:

		pos.x = mouse_pos.x - margin.size.x - 16

	if pos.y + margin.size.y > viewport_size.y:

		pos.y = mouse_pos.y - margin.size.y - 16

	global_position = pos


func on_show() -> void:

	visible = true

	is_visible_description = true


func on_hide() -> void:

	visible = false

	is_visible_description = false
