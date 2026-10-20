'use client'

import { useState } from 'react';
import { motion } from "framer-motion";
import { Eye, EyeOff, Mail} from "lucide-react";
import { Button } from '@/components/ui/button';
import { Input} from '@/components/ui/input';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';

export default function Home() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)

  const [error, setError] = useState('')

async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
  e.preventDefault()
  setError('')

  const res = await fetch('http://localhost:5246/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password }),
  })

  if (!res.ok) {
    setError('Wrong username or password')
    return
  }

  const data = await res.json()
  localStorage.setItem('token', data.token)
}
  
  return (
    <div className='min-h-screen bg-gradient-to-br from-primary-50 to-primary-100 flex items-center justify-center p-4'>
      <motion.div
      initial={{opacity: 0, y: -20}}
      animate={{opacity: 1, y: 0}}
      transition={{duration: 0.5}}
      className='w-full max-w-md'
      >
        <div className='bg-white rounded-2xl shadow-xl p-8 space-y-6'>
          <div className='text-center space-y-2'> 
            <h1 className='text-3xl font-bold tracking-tighter'>Welcome</h1>
            <p className='text-muted-foreground'>Enter your creditials to create a Pulse</p>
          </div>
          <form onSubmit={handleSubmit} className='space-y-4'>
            <div className='space-y-2'>
              <Label htmlFor='username'>Username</Label>
              <Input
              id="username"
              type="text"
              placeholder='JohnDoe'
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              />
      
            </div>
             <div className='space-y-2'>
              <Label htmlFor='password'>Password</Label>
                <div className='relative'>
                  <Input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className='pr-10'
                  required
                  />
                  <Button
                  type='button'
                  onClick={() => setShowPassword(!showPassword )}
                  variant='ghost' size='icon'
                  className='absolute right-1 top-1/2 transform -translate-y-1/2 text-gray-500 hover:text-gray-700'
                  >
                    {showPassword ? <EyeOff size={20} /> : <Eye size={20} /> }
                  </Button>
              </div>
            </div>
            <div className='flex items-center justify-between'>
              <div className='flex items-center space-x-2'>
                <Checkbox id='remember' />
                <Label htmlFor='remember'>Remember me</Label>
              </div>
              <a href='#' className='text-sm text-primary-500 hover:text-primary-600'>
                Forgot password?
                </a>
            </div>
            <div>
              {error && <p className='text-sm text-red-600'>{error}</p>}
                <Button type='submit' className='w-full'>
                  Sign in
                </Button>
              </div>
          </form>
        </div>
      
      </motion.div>
    </div>
  );
}
