# -*- coding: utf-8 -*-
"""
    File Name: SnakeAI.py
    Author: Ari Madian
    Created: April 9, 2018 5:55 PM
    Python Version: 3.6
    SnakeAI.py - Part of Machine-Learning Repo
    Repo: github.com/akmadian/SnakeAI
"""
from unityagents import UnityEnvironment
from unityagents import BrainInfo
import tensorflow as tf

import arrow


print(arrow.utcnow().to('US/Pacific'))
print('Imported')


env_name = "SnakeAI"
train_mode = True
env = UnityEnvironment(file_name=env_name)
default_brain = env.brain_names[0]
brain = env.brains[default_brain]
brain_info = None



env.reset()
print('env.reset()')
binfo = env.step(1)
print(type(binfo))
b_ = binfo['Brain']
print(b_.vector_observations)
for key, value in binfo.items():
	print(str(key) + ' - ' + str(value))
	print(vars(value))
print('env.step()')
print('__________')


env.reset(train_mode=train_mode, config=None)

env.close()


