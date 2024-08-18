// src/app/page.jsx
import Link from 'next/link';

export default function Home() {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-2 bg-gradient-to-r from-green-400 to-blue-500 text-white">
      <h1 className="text-5xl font-extrabold mb-8 drop-shadow-lg">Welcome to My Website</h1>
      <div className="space-y-4">
        <Link href="/write" className="px-6 py-3 bg-blue-600 text-white rounded-full shadow-lg hover:bg-blue-700 transition duration-300 transform hover:scale-105">
          Write
        </Link>
        <Link href="/read" className="px-6 py-3 bg-green-600 text-white rounded-full shadow-lg hover:bg-green-700 transition duration-300 transform hover:scale-105">
          Read
        </Link>
        <Link href="/viewList" className="px-6 py-3 bg-red-600 text-white rounded-full shadow-lg hover:bg-red-700 transition duration-300 transform hover:scale-105">
          View List
        </Link>
      </div>
    </div>
  );
}
