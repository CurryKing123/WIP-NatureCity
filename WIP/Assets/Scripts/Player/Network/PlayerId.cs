using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerId : NetworkBehaviour
{
    [SyncVar] public int userId;
}
