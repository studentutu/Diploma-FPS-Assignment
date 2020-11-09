﻿using UnityEngine;
using Rewired;
using UnityEngine.UI;
using FPS;

[SerializeField]
public class WeaponHandler : MonoBehaviour
{
    #region |Variables
    // * //
    #region ||Global
    [HideInInspector]
    public static GunTypes currentType;
    #endregion

    #region ||General
    public int selectedWeapon = 0;
    [SerializeField] private Image[] gunIcons;
    #endregion

    #region ||References
    private Player _player;
    private FpsCustom _custom;
    #endregion

    #region ||Flash Light
    [SerializeField] private GameObject flashLight;
    #endregion
    #endregion

    #region |Setup
    private void Awake()
    {
        //Setup automated refrences
        _custom = GetComponentInParent<FpsCustom>();
    }

    private void Start()
    {
        //Select first weapon to start
        SelectWeapon();

        //This setup is done in start to ensure the custom script has completed is awake method
        _player = _custom._player;
    }
    #endregion

    private void Update()
    {
        //Our previous selection of weapon is what we have at the start of each frame
        int prevSelection = selectedWeapon;

        #region |ScrollWheel & D-Pad
        //* Increase Selection
        if (GetSwitchInputs() > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        //* Decrease Selection
        else if (GetSwitchInputs() < 0)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }
        #endregion

        #region |Number Changes
        if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 1)
            selectedWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            selectedWeapon = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            selectedWeapon = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            selectedWeapon = 3;

        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
            selectedWeapon = 4;
        #endregion

        if (prevSelection != selectedWeapon)
            SelectWeapon();

        if (_player.GetButtonDown("Toggle Light"))
            ToggleLight();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetSwitchInputs()
    {
        if (_player == null)
            _player = _custom._player;

        int input = 0;

        input = (Input.GetAxis("Mouse ScrollWheel") < 0) ? -1 : ((Input.GetAxis("Mouse ScrollWheel") > 0) ? 1 : input);

        input = (_player.GetButtonDown("Switch Down")) ? -1 : ((_player.GetButtonDown("Switch Up")) ? 1 : input);

        return input;
    }

    #region |Apply Selection
    /// <summary>
    /// Used to select the weapon using the variable \link #selectedWeapon selectedWeapon.
    /// </summary>
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                currentType = weapon.GetComponent<Gun>().gunType;
                gunIcons[i].color = Color.white;
            }
            else
            {
                weapon.gameObject.SetActive(false);
                gunIcons[i].color = new Color32(178, 178, 178, 255);
            }

            i++;
        }
    }
    #endregion

    /// <summary>
    /// This does something cool.
    /// </summary>
    /// Usage:
    /// @code
    /// public float GetValueOf(float _myParam)
    /// {
    ///     return _myParam;
    /// }
    /// @endcode
    /// <param name="_myParam">A float value to return</param>
    public float GetValueOf(float _myParam)
    {
        return _myParam;
    }

    void ToggleLight()
    {
        flashLight.SetActive(!flashLight.activeSelf);
    }
}

[System.Serializable]
public enum GunTypes
{
    Rifle,
    Pistol,
    Shotgun,
    Sniper
}