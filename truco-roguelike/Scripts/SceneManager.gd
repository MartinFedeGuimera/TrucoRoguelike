extends Node


func load(scene_path : String) -> void:
	get_tree().change_scene_to_file(scene_path)
