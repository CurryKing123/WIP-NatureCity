UPDATE NATURE.resource_node
SET resource_node_name=?, 
resource_amount=?, 
gathering_time=?,
respawn_time=?,
resource_node=?,
resource_id=?,
req_tool=?

WHERE resource_node_id=?;