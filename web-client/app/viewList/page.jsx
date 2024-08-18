// src/app/viewList/page.jsx
"use client";

import { useState } from 'react';
import axios from 'axios';

export default function ViewList() {
  const [messages, setMessages] = useState([]);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  const fetchMessages = async (start, end) => {
    try {
      const response = await axios.get(`${process.env.API_URL}/api/Message`, {
        params: {
          startDate: start,
          endDate: end,
        },
      });
      setMessages(response.data);
    } catch (error) {
      console.error('Error fetching messages:', error);
    }
  };

  const handleLast10Minutes = () => {
    const end = new Date().toISOString();
    const start = new Date(Date.now() - 10 * 60 * 1000).toISOString();
    fetchMessages(start, end);
  };

  const handleCustomRange = (e) => {
    e.preventDefault();
    fetchMessages(startDate, endDate);
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-2 bg-gradient-to-r from-green-400 to-blue-500 text-white">
      <h1 className="text-5xl font-extrabold mb-8 drop-shadow-lg">View Messages</h1>
      <div className="w-full max-w-md space-y-4 bg-white p-4 rounded shadow-md text-black">
        <button
          onClick={handleLast10Minutes}
          className="w-full px-4 py-2 bg-blue-600 text-white rounded-full shadow-lg hover:bg-blue-700 transition duration-300 transform hover:scale-105 mb-4"
        >
          Show Messages from Last 10 Minutes
        </button>
        <form onSubmit={handleCustomRange} className="space-y-4">
          <input
            type="datetime-local"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className="w-full p-2 border rounded"
            placeholder="Start Date"
          />
          <input
            type="datetime-local"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="w-full p-2 border rounded"
            placeholder="End Date"
          />
          <button
            type="submit"
            className="w-full px-4 py-2 bg-green-600 text-white rounded-full shadow-lg hover:bg-green-700 transition duration-300 transform hover:scale-105"
          >
            Show Messages
          </button>
        </form>
        <div className="mt-4">
          {messages.map((msg) => (
            <div key={msg.id} className="mb-2">
              <span className="font-bold">Serial Number:</span> {msg.serialNumber}<br />
              <span className="font-bold">Date Created:</span> {new Date(msg.dateCreated).toLocaleString(undefined, { timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone })}<br />
              <span className="font-bold">Content:</span> {msg.content}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
