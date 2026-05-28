extends Control
class_name PlayerHealthBar


var progress_bar : ProgressBar

var health_label : Label


var displayed_value : float


func _ready() -> void:

	progress_bar = get_node("HealthBar")

	health_label = get_node("HealthText")

	displayed_value = PlayerData.instance.health


func _process(delta : float) -> void:

	var target : float = PlayerData.instance.health

	displayed_value = lerpf(
		displayed_value,
		target,
		8.0 * delta
	)

	if abs(displayed_value - target) < 0.5:

		displayed_value = target

	progress_bar.value = (
		displayed_value
		/
		PlayerData.instance.max_health
	) * 100.0

	health_label.text = str(
		roundi(displayed_value)
	)
