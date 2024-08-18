// src/app/read/page.jsx
"use client";

import { useState, useEffect } from 'react';

export default function Read() {
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    const socket = new WebSocket(`${process.env.API_URL.replace('http', 'ws')}/ws`);

    socket.onopen = () => {
      console.log('WebSocket connection established');
    };

    socket.onmessage = (event) => {
      console.log('Message from server:', event.data);
      const newMessage = JSON.parse(event.data);
      setMessages((prevMessages) => [...prevMessages, newMessage]);
    };

    socket.onclose = () => {
      console.log('WebSocket connection closed');
    };

    return () => {
      socket.close();
    };
  }, []);

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-2 bg-gradient-to-r from-green-400 to-blue-500 text-white">
      <h1 className="text-5xl font-extrabold mb-8 drop-shadow-lg">Read Messages</h1>
      <div className="w-full max-w-md space-y-4 bg-white p-4 rounded shadow-md text-black">
        {messages.map((msg) => (
          <div key={msg.serialNumber} className="mb-2">
            <span className="font-bold">{msg.serialNumber}:</span> {msg.text}
          </div>
        ))}
      </div>
    </div>
  );
}
