using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class GenerateSpiner : MonoBehaviour
{
    [SerializeField] public Texture2D _texture;
    [SerializeField] int resolution = 1080;
    [SerializeField] TextureWrapMode _wrapMode = TextureWrapMode.Clamp;

    [SerializeField] Color32[] Colors = { new Color32(255, 0, 0, 255), new Color32(0, 255, 0, 255), new Color32(0, 0, 255, 255) };
    [SerializeField] Color32 innerColor = new Color32(150, 150, 150, 255);
    private Color32 AlphaColor = new Color32(0, 0, 0, 0);

    [SerializeField] List<UnityEvent> Event;
    [SerializeField] float innerRadPersent = 5;

    [SerializeField] GameObject holder;

    private float scrInRad;

    private Rigidbody _rb;
    private Vector3 holderOffset;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        holderOffset = holder.transform.position - transform.position;
    }

    private void OnValidate()
    {
        ChangeEvents();
        GenerateTexture();
    }

    public void ReturnHolder()
    {
        holder.transform.rotation = Quaternion.Euler(holder.transform.rotation.eulerAngles[0] * Vector3.right + new Vector3(0, 90, -90));
        holder.transform.localPosition = holderOffset + transform.position;
    }

    private float scrDist(float x1, float y1, float x2, float y2)
    {
        float res;
        res = (x2 - x1) * (x2 - x1);
        res += (y2 - y1) * (y2 - y1);
        return res;
    }

    private void ChangeEvents()
    {
        if (Event.Count < Colors.Length)
        {
            while(Event.Count < Colors.Length)
            {
                Event.Add(new UnityEvent());
            }
        }
        if(Event.Count > Colors.Length)
        {
            while (Event.Count > Colors.Length)
            {
                Event.RemoveAt(Event.Count - 1);
            }
        }
    }
    private void GenerateTexture()
    {
        if (_texture == null)
        {
            _texture = new Texture2D(resolution, resolution);
            _texture.wrapMode = _wrapMode;
            _texture.filterMode = FilterMode.Bilinear;
        }
        scrInRad = Mathf.Pow(resolution / 2 * innerRadPersent / 100, 2);
        Debug.Log(scrInRad);

        GetComponent<Renderer>().sharedMaterial.mainTexture = _texture;
        if (_texture.width != resolution)
        {
            _texture.Resize(resolution, resolution);
        }
        GenerateSectors();
        RoundCoreners();
    }
    private void RoundCoreners()
    {
        for (int i = 0; i < resolution / 2; i++)
        {
            for (int j = 0; j < resolution / 2 - i; j++)
            {
                if (scrDist(i, j, resolution / 2f, resolution / 2f) > resolution * resolution / 4)
                {
                    _texture.SetPixel(i, j, AlphaColor);
                    _texture.SetPixel(i, resolution - 1 - j, AlphaColor);
                    _texture.SetPixel(resolution - 1 - i, j, AlphaColor);
                    _texture.SetPixel(resolution - 1 - i, resolution - 1 - j, AlphaColor);
                }
            }
        }
        _texture.Apply();
    }

    private void GenerateSectors()
    {
        int count = Colors.Length;
        float SecWidth = Mathf.PI * 2 / count;
        float scuareMiddle = resolution / 2;
        int innerCount = 0;
        for(int i = 0; i < resolution; i++)
        {
            for(int j = 0; j < resolution; j++)
            {
                float x = (j - scuareMiddle);
                float y = (i - scuareMiddle);
                if(scrDist(x,y, 0, 0) < scrInRad)
                {
                    _texture.SetPixel(i, j, innerColor);
                    innerCount++;
                }
                else {
                    float sin = y / Mathf.Sqrt(x * x + y * y);
                    float Angle = Mathf.Asin(sin);
                    if (x < 0)
                    {
                        Angle = Mathf.PI - Angle;
                    }

                    Angle += SecWidth / 2;
                    if (Angle < 0)
                    {
                        Angle += Mathf.PI * 2;
                    }

                    int colorIndex = (int)Mathf.Floor(Angle / SecWidth);
                    colorIndex %= count;
                    if (colorIndex < 0)
                    {
                        colorIndex += count;
                    }
                    _texture.SetPixel(i, j, Colors[colorIndex]);
                }
            }
        }
        _texture.Apply();
        Debug.Log(innerCount);
    }
    
}
