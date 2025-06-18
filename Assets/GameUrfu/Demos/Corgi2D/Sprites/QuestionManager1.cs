using System.Collections;
using UnityEngine;
using TMPro; // Не забудьте добавить это, если вы используете TextMeshPro

public class QuestionManager1 : MonoBehaviour
{
    public TextMeshPro questionText; // Ссылка на текст вопроса
    public TextMeshPro timerText; // Ссылка на текст таймера
    public GameObject[] platforms; // Массив платформ, которые нужно активировать или деактивировать
    public GameObject questionTrigger; // Ссылка на объект QuestionTrigger
    public GameObject yellowKeyPicker; // Ссылка на YellowKeyPicker
    public GameObject canvasToHide; // Ссылка на канвас, который нужно показать/скрыть

    public float questionDuration = 15f; // Время на ответ
    private bool questionActive = false; // Флаг активности вопроса
    private int currentQuestionIndex = 0; // Индекс текущего вопроса
    private Coroutine timerCoroutine; // Ссылка на корутину таймера

    private string[] questions = {
        "Криптовалюта - это физическая валюта?",
        "Блокчейн - это сеть онлайн магазинов?",
        "Биткоин - это децентрализованная валюта?",
        "Майнинг - это покупка в интернете?"
    };

    private bool[] answers = { false, false, true, false }; // Массив правильных ответов

    private void Start()
    {
        // Скрываем текстовые элементы и платформы в начале
        questionText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        HidePlatforms(); // Скрываем платформы при старте

        if (yellowKeyPicker != null)
        {
            yellowKeyPicker.SetActive(false); // Скрываем YellowKeyPicker
        }

        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false); // Скрываем канвас на старте
        }
    }

    public void StartQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            questionActive = true;
            questionText.gameObject.SetActive(true); // Активируем текст вопроса
            timerText.gameObject.SetActive(true); // Активируем таймер
            questionText.text = questions[currentQuestionIndex]; // Устанавливаем текст вопроса

            // Запускаем таймер
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine); // Останавливаем старый таймер, если он существует
            }
            timerCoroutine = StartCoroutine(StartTimer(questionDuration));
        }
    }

    private IEnumerator StartTimer(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timerText.text = timer.ToString("F0"); // Обновляем текст таймера
            yield return new WaitForSeconds(1f);
            timer--;
        }

        // Если время истекло, сбрасываем вопрос
        questionActive = false;
        questionText.gameObject.SetActive(false); // Скрываем текст вопроса
        timerText.gameObject.SetActive(false); // Скрываем таймер
        currentQuestionIndex = 0; // Сбрасываем индекс
        HidePlatforms(); // Скрываем платформы, если время истекло
    }

    public void Answer(bool playerAnswer)
    {
        if (playerAnswer == answers[currentQuestionIndex]) // Проверяем ответ
        {
            currentQuestionIndex++;
            questionActive = false;
            questionText.gameObject.SetActive(false); // Скрываем текст вопроса
            timerText.gameObject.SetActive(false); // Скрываем таймер

            if (currentQuestionIndex < questions.Length)
            {
                StartQuestion(); // Запускаем следующий вопрос
            }
            else
            {
                Debug.Log("Поздравляем! Вы ответили на все вопросы.");
                HidePlatforms(); // Скрываем платформы, если все ответы правильные
                HideQuestionTrigger(); // Скрываем триггер
                ActivateYellowKeyPicker(); // Активируем YellowKeyPicker
                HideCanvas(); // Скрываем канвас по окончанию игры
            }
        }
        else
        {
            Debug.Log("Неправильный ответ. Попробуйте снова.");
            questionActive = false;
            questionText.gameObject.SetActive(false); // Скрываем текст вопроса
            timerText.gameObject.SetActive(false); // Скрываем таймер
            currentQuestionIndex = 0; // Сбрасываем индекс
            HidePlatforms(); // Скрываем платформы, если ответ неправильный
        }
    }

    private void HidePlatforms()
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false); // Деактивируем платформы
        }
    }

    private void HideQuestionTrigger()
    {
        if (questionTrigger != null)
        {
            questionTrigger.SetActive(false); // Деактивируем QuestionTrigger
        }
    }

    private void ActivateYellowKeyPicker()
    {
        if (yellowKeyPicker != null)
        {
            yellowKeyPicker.SetActive(true); // Активируем YellowKeyPicker
            Debug.Log("YellowKeyPicker активирован!");
        }
        else
        {
            Debug.LogError("YellowKeyPicker не назначен!");
        }
    }

    private void ShowCanvas()
    {
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(true); // Показываем канвас
        }
    }

    private void HideCanvas()
    {
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false); // Скрываем канвас
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, если игрок вошел в триггер
        if (collision.CompareTag("Player")) // Убедитесь, что тег игрока действительно "Player"
        {
            ShowCanvas(); // Показываем канвас при входе триггера
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Оставим это пустым, чтобы канвас не скрывался при выходе игрока из триггера.
    }
}