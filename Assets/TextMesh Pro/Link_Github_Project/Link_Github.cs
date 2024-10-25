using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        // Obtém automaticamente o componente TextMeshProUGUI anexado ao GameObject
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing!");
            return;
        }

        // Define o texto com a tag <link>
        textMeshPro.text = "Developed by <link=\"https://github.com/Vinishireis\">Vinishireis</link>";
    }

    // Este método será chamado quando o texto for clicado
    public void OnPointerClick(PointerEventData eventData)
    {
        if (textMeshPro == null) return;

        // Converte a posição do mouse para a tela de coordenadas do mundo
        Vector3 worldPoint = Input.mousePosition;

        // Verifica se o clique está em um link
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, worldPoint, null);
        if (linkIndex != -1) // Verifica se clicou em um link
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            Debug.Log("Link clicado: " + linkID);

            // Abre o link
            Application.OpenURL(linkID);
        }
    }
}
