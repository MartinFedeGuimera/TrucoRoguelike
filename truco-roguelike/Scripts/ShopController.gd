extends Node
class_name ShopController


@export var game_data : GameData

var seed : RandomNumberGenerator


@export var description_controller : DescriptionController


@export_group("Shop Settings")

@export var max_relics : int = 3

@export var max_consumables : int = 2

@export var reroll_price : int


@export_group("Data Settings")

@export var relics_data : Array[RelicController] = []

var relics : Array[RelicController] = []


@export var relic_product_scene : PackedScene


@export var consumables_data : Array[Consumable] = []

var consumables : Array[Consumable] = []


@export var consumable_product_scene : PackedScene


@export var relic_scene : PackedScene

@export var consumable_scene : PackedScene


var drawn_relics : Array[RelicProduct] = []

var drawn_consumables : Array[ConsumableProduct] = []


var relics_container : HBoxContainer

var consumables_container : VBoxContainer


var money_label : Label

var relics_products_container : HBoxContainer

var consumables_products_container : HBoxContainer


@export_group("Sounds")

var sfx_player : AudioStreamPlayer2D

@export var buy_sound : AudioStream


var rng : RandomNumberGenerator = RandomNumberGenerator.new()


func _ready() -> void:

	money_label = get_node("MoneyLabel")

	relics_products_container = get_node(
		"ShopContent/ProductsContainer/RelicsContainer"
	)

	consumables_products_container = get_node(
		"ShopContent/ProductsContainer/ConsumablesContainer"
	)

	relics_container = get_node(
		"PlayerData/RelicsContainer"
	)

	consumables_container = get_node(
		"PlayerData/ConsumablesContainer"
	)

	sfx_player = get_node("SfxPlayer")

	description_controller.set_up()

	PlayerData.instance.data_changed.connect(
		update_player_data_ui
	)

	seed = game_data.seed

	rng.randomize()

	for relic in relics_data:

		relics.append(
			relic.duplicate(true)
		)

	for consumable in consumables_data:

		consumables.append(
			consumable.duplicate(true)
		)

	update_player_data_ui()

	set_up()


func _exit_tree() -> void:

	if PlayerData.instance.data_changed.is_connected(
		update_player_data_ui
	):
		PlayerData.instance.data_changed.disconnect(
			update_player_data_ui
		)


func clear_player_data_ui() -> void:

	for child in relics_container.get_children():

		child.queue_free()

	for child in consumables_container.get_children():

		child.queue_free()


func update_player_data_ui() -> void:

	money_label.text = "$" + str(PlayerData.instance.money)

	clear_player_data_ui()

	for relic in PlayerData.instance.relics:

		var relic_view : RelicView = (
			relic_scene.instantiate()
		)

		relic_view.set_up(
			relic,
			description_controller
		)

		relics_container.add_child(relic_view)

	for consumable in PlayerData.instance.consumables:

		var controller : ConsumableController = (
			consumable_scene.instantiate()
		)

		controller.set_up_shop(
			consumable,
			description_controller
		)

		consumables_container.add_child(controller)


func set_up() -> void:

	shuffle_relics()

	draw_relics()

	shuffle_consumables()

	draw_consumables()


func on_reroll() -> void:

	if PlayerData.instance.money >= reroll_price:

		reroll_price += 2

		sfx_player.pitch_scale = rng.randf_range(
			0.8,
			1.1
		)

		sfx_player.stream = buy_sound

		sfx_player.play()

		for relic in relics_products_container.get_children():

			relic.queue_free()

		drawn_relics.clear()

		for consumable in consumables_products_container.get_children():

			consumable.queue_free()

		drawn_consumables.clear()

		set_up()

	else:

		print("Not Enough Money")


func on_continue() -> void:

	SceneManager.load(
		"res://Scenes/Screens/Game.tscn"
	)


func try_buy_relic(
	relic_data : RelicController
) -> void:

	if (
		PlayerData.instance.money >= relic_data.price
		and
		PlayerData.instance.relics.size() + 1
		<= PlayerData.instance.max_relics
	):

		PlayerData.instance.money -= relic_data.price

		sfx_player.pitch_scale = rng.randf_range(
			0.8,
			1.1
		)

		sfx_player.stream = buy_sound

		sfx_player.play()

		print(relic_data.name + " Added to Relics")

		PlayerData.instance.relics.append(relic_data)

		for i in range(drawn_relics.size() - 1, -1, -1):

			if drawn_relics[i].get_data().name == relic_data.name:

				drawn_relics.remove_at(i)

		for i in range(relics.size() - 1, -1, -1):

			if relics[i].name == relic_data.name:

				relics.remove_at(i)

		for child in relics_products_container.get_children():

			if (
				child is RelicProduct
				and
				child.get_data().name == relic_data.name
			):
				child.queue_free()

		update_player_data_ui()

	else:

		print("Not Enough Money")


func try_buy_consumable(
	consumable_data : Consumable
) -> void:

	if (
		PlayerData.instance.money >= consumable_data.price
		and
		PlayerData.instance.consumables.size() + 1
		<= PlayerData.instance.max_consumables
	):

		PlayerData.instance.money -= consumable_data.price

		sfx_player.pitch_scale = rng.randf_range(
			0.8,
			1.1
		)

		sfx_player.stream = buy_sound

		sfx_player.play()

		PlayerData.instance.consumables.append(
			consumable_data
		)

		for i in range(
			drawn_consumables.size() - 1,
			-1,
			-1
		):

			if (
				drawn_consumables[i]
				.get_data()
				.name
				==
				consumable_data.name
			):
				drawn_consumables.remove_at(i)

		for i in range(
			consumables.size() - 1,
			-1,
			-1
		):

			if consumables[i].name == consumable_data.name:

				consumables.remove_at(i)

		for child in consumables_products_container.get_children():

			if (
				child is ConsumableProduct
				and
				child.get_data().name
				==
				consumable_data.name
			):
				child.queue_free()

		update_player_data_ui()

	else:

		print("Not Enough Money")


func shuffle_relics() -> void:

	for i in range(relics.size() - 1, 0, -1):

		var random_index = seed.randi_range(0, i)

		var current_relic = relics[i]

		relics[i] = relics[random_index]

		relics[random_index] = current_relic


func draw_relics() -> void:

	while (
		drawn_relics.size() < max_relics
		and
		relics.size() > 0
	):

		var new_relic_data = relics[0]

		relics.remove_at(0)

		var is_repeated := false

		for owned_relic in PlayerData.instance.relics:

			if owned_relic.name == new_relic_data.name:

				is_repeated = true

				break

		if is_repeated:
			continue

		var new_relic : RelicProduct = (
			relic_product_scene.instantiate()
		)

		new_relic.set_up(
			new_relic_data,
			self,
			description_controller
		)

		drawn_relics.append(new_relic)

		relics_products_container.add_child(new_relic)


func shuffle_consumables() -> void:

	for i in range(consumables.size() - 1, 0, -1):

		var random_index = seed.randi_range(0, i)

		var current_consumable = consumables[i]

		consumables[i] = consumables[random_index]

		consumables[random_index] = current_consumable


func draw_consumables() -> void:

	var amount = min(
		max_consumables,
		consumables.size()
	)

	for i in range(amount):

		var new_consumable_data = consumables[0]

		var is_repeated := false

		for owned in PlayerData.instance.consumables:

			if owned.name == new_consumable_data.name:

				is_repeated = true

				break

		if !is_repeated:

			var new_consumable : ConsumableProduct = (
				consumable_product_scene.instantiate()
			)

			new_consumable.set_up(
				new_consumable_data,
				self,
				description_controller
			)

			drawn_consumables.append(
				new_consumable
			)

			consumables_products_container.add_child(
				new_consumable
			)

			consumables.remove_at(0)
