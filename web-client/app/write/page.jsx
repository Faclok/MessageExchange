// src/app/write/page.jsx
"use client";

import { useState, useEffect } from 'react';
import axios from 'axios';

export default function Write() {
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);
  const [serialNumber, setSerialNumber] = useState(1);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (message.length > 128) {
      alert('Message length should not exceed 128 characters');
      return;
    }

    const newMessage = {
      serialNumber,
      text: message,
    };

    try {
      await axios.post(`${process.env.API_URL}/api/Message`, newMessage);
      setMessages([...messages, newMessage]);
      setSerialNumber(serialNumber + 1);
      setMessage('');
    } catch (error) {
      console.error('Error sending message:', error);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-2 bg-gradient-to-r from-green-400 to-blue-500 text-white">
      <h1 className="text-5xl font-extrabold mb-8 drop-shadow-lg">Write a Message</h1>
      <div className="w-full max-w-md space-y-4">
        <div className="bg-white p-4 rounded shadow-md text-black">
          {messages.map((msg) => (
            <div key={msg.serialNumber} className="mb-2">
              <span className="font-bold">{msg.serialNumber}:</span> {msg.text}
            </div>
          ))}
        </div>
        <form onSubmit={handleSubmit} className="bg-white p-4 rounded shadow-md">
          <textarea
            className="w-full p-2 border rounded mb-4 text-black"
            maxLength="128"
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            placeholder="Enter your message (max 128 characters)"
          />
          <button
            type="submit"
            className="w-full px-4 py-2 bg-blue-600 text-white rounded-full shadow-lg hover:bg-blue-700 transition duration-300 transform hover:scale-105"
          >
            Send Message
          </button>
        </form>
      </div>
    </div>
  );
}
