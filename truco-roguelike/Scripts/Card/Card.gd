extends Resource
class_name Card


enum CardSuit {
	NONE,
	ESPADA,
	COPA,
	ORO,
	BASTO
}


@export var name : String

@export var value : int

@export var envido_value : int

@export var mult : int = 1

@export var suit : CardSuit

@export var texture : Texture2D
