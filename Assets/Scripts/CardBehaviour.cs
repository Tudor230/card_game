using UnityEngine;
using Photon.Pun;
using Photon.Realtime;  // Needed for Player class and ownership callbacks

public class CardBehaviour : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private Vector3 offset;
    private bool isDragging = false;

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this); // Register for ownership callbacks
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this); // Unregister ownership callbacks
    }

    private void OnMouseDown()
    {
        if (!photonView.IsMine)
        {
            // If the player doesn't own the card, request ownership
            photonView.RequestOwnership();
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            offset = transform.position - mousePos;
        }
    }

    private void OnMouseDrag()
    {
        if (photonView.IsMine) // Only allow movement if this player owns the card
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    // Handle the ownership request
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView == photonView)
        {
            Debug.Log("Ownership requested by: " + requestingPlayer.NickName);
            // Optionally, handle ownership request approval logic here
        }
    }

    // Callback when ownership is transferred
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView == photonView)
        {
            Debug.Log("Ownership transferred to: " + photonView.Owner.NickName);
        }
    }

    // Handle failed ownership transfer
    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        if (targetView == photonView)
        {
            Debug.Log("Ownership transfer failed for: " + photonView.Owner.NickName);
        }
    }
}
