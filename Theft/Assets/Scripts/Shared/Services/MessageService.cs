using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Shared {

    /**
     * A service that shows text messages to the user.
     */
    public class MessageService: MonoBehaviour {

        /** Singleton service instance */
        private static MessageService service = null;

        /** Message queue */
        private static Queue<Message> queue = new Queue<Message>();

        /** Text field for the messsages */
        private static Text textField = null;


        /**
         * A message on the queue
         */
        private class Message {
            public float delay = 0.0f;
            public string text = string.Empty;
        }


        /**
         * Makes the service a singleton.
         */
        private void Awake() {
            if (service == null) {
                textField = GetComponentInChildren<Text>(true);
                StartCoroutine(ShowNextMessage());
                service = this;
            } else if (service != this) {
                Destroy(gameObject);
            }
        }


        /**
         * Show a text message.
         */
        public static void Push(string text, float delay = 0.0f) {
            queue.Enqueue(new Message { text = text, delay = delay });
        }


        /**
         * Coroutine that displays the enqueued messages.
         */
        private IEnumerator ShowNextMessage() {
            while (enabled) {
                if (queue.Count > 0) {
                    Message message = queue.Dequeue();
                    yield return new WaitForSeconds(message.delay);

                    textField.text = message.text;
                    textField.gameObject.SetActive(true);
                    yield return new WaitForSeconds(5.0f);

                    textField.gameObject.SetActive(false);
                }

                yield return null;
            }
        }
    }
}
