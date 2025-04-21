import {
  ScrollView,
  StyleSheet,
  Text,
  View,
} from 'react-native'
import React from 'react'
import "@/global.css"

import Input from "@/components/ui/input"
import Button from "@/components/ui/button"
import MorentHeroSection from '@/components/morent_hero'

const LandingScreen = () => {
  return (
    <View className='flex-1'>
      <ScrollView>
        <View
          className="flex flex-row flex-wrap mb-8 mx-5"
        >
          <Input className="flex-1"/>
          <Button
            icon="options-outline"
            iconPosition="only"
            variant='secondary'
            size='md'
          />
        </View>
        <MorentHeroSection />
      </ScrollView>
    </View>
  )
}

export default LandingScreen 

const styles = StyleSheet.create({})
