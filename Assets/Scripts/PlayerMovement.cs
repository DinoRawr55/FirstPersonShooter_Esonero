using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController cc;

    public float speed = 12f;

    public float gravity = -9.81f;

    Vector3 velocity;

    private bool groundedPlayer;

    [SerializeField] private float jumpHeight = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
{
    groundedPlayer = cc.isGrounded;

    if (groundedPlayer && velocity.y < 0)
    {
        // Quando il personaggio è a terra, resettiamo la velocità verticale a -2f
        // Non la settiamo a 0 perché piccola gravità mantiene il player "attaccato" al terreno
        // Prevenisce il player di "fluttuare" o separarsi dal suolo tra un frame e l'altro
        velocity.y = -2f;
    }

    // 1. Calcolo movimento orizzontale
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");
    Vector3 move = transform.right * x + transform.forward * z;

    // 2. Logica del Salto
    if (Input.GetButtonDown("Jump") && groundedPlayer)
    {
        // Formula fisica: v = sqrt(2 * gravità * altezza_desiderata)
        // Calcola la velocità iniziale (m/s) necessaria per raggiungere jumpHeight
        // Il moltiplicatore -3.0f modula la forza del salto rispetto alla gravità
        // Usiamo -3.0f * gravity perché gravity è negativa (-9.81), quindi il risultato è positivo
        velocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    }

    // 3. Applichiamo la gravità alla velocità verticale
    // STEP 1 di integrazione fisica: Accelerazione × Tempo = Velocità
    // gravity (m/s²) × Time.deltaTime (s) = velocità accumulata (m/s)
    // velocity.y accumula questa velocità ogni frame
    velocity.y += gravity * Time.deltaTime;

    // 4. UNICA CHIAMATA MOVE: combina orizzontale e verticale
    // STEP 2 di integrazione fisica: Velocità × Tempo = Spostamento
    // (move * speed + velocity) è la velocità totale (m/s)
    // Moltiplicare per Time.deltaTime converte in spostamento (m)
    // Così il movimento è indipendente dal framerate
    cc.Move((move * speed + velocity) * Time.deltaTime);
}
}
