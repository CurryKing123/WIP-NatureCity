UPDATE NATURE.user
SET    user_name = ?,
       character_race = ?,
       equip_item_1 = ?,
       equip_item_2 = ?,
       equip_item_3 = ?,
       equip_item_4 = ?,
       equip_item_5 = ?,
       player_hp = ?,
       player_stamina = ?,
       player_status = ?
WHERE  user_id = ?; 