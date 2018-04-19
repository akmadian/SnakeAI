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

# ML-Agents Variables
env_name = "SnakeAI"
train_mode = True
env = UnityEnvironment(file_name=env_name)
default_brain = env.brain_names[0]
brain = env.brains[default_brain]
brain_info = None

# Tensorflow Variables
n_nodes_hl1 = 15
n_nodes_hl2 = 20
n_nodes_hl3 = 15
n_actions = 4
n_epochs = 10

x = tf.placeholder('float', [None, 17])
y = tf.placeholder('float')

env.reset()
print('env.reset()')
binfo = env.step(1)['Brain']
vector_observations = binfo.vector_observations[0][:-1]
train_neural_network(vector_observations)

def neural_network_model(data):
	hidden_l1 = {'weights': tf.Variable(tf.random_normal([17, n_nodes_hl1])),
				 'biases': tf.Variable(tf.random_normal([n_nodes_hl1]))}
	hidden_l2 = {'weights': tf.Variable(tf.random_normal([n_nodes_hl1, n_nodes_hl2])),
				 'biases': tf.Variable(tf.random_normal([n_nodes_hl2]))}
	hidden_l3 = {'weights': tf.Variable(tf.random_normal([n_nodes_hl2, n_nodes_hl3])),
				 'biases': tf.Variable(tf.random_normal([n_nodes_hl3]))}

	output_layer = {'weights': tf.Variable(tf.random_normal([n_nodes_hl3, n_actions])),
				    'biases': tf.Variable(tf.random_normal(n_actions))}

	l1 = tf.add(tf.matmul(data, hidden_l1['weights']), hidden_l1['biases'])
	l1 = tf.nn.relu(l1)

	l2 = tf.add(tf.matmul(l1, hidden_l2['weights']), hidden_l2['biases'])
	l2 = tf.nn.relu(l2)

	l3 = tf.add(tf.matmul(l2, hidden_l3['weights']), hidden_l3['biases'])
	l3 = tf.nn.relu(l3)

	output = tf.add(tf.matmul(l3, output_layer['weights']) + output_layer['biases'])

	return output

def train_neural_network(x):
	prediction = neural_network_model(x)
	cost = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(prediction, y))
	optimizer = tf.train.AdamOptimizer().minimize(cost)

	with tf.Session() as sess:
		sess.run(tf.initialize_all_variables())

		for epoch in range(n_epochs):
			e_loss = 0
			for _ in range(1):
				e_x, e_y = None
				_, c = sess.run([optimizer, cost], feed_dict = {x: e_x, y: e_y})
				e_loss += c
			print('Epoch #' + str(epoch) + ' of ' + str(n_epochs) + ' complete - Loss: ' + str(e_loss))

		correct = tf.equal(tf.argmax(prediction, 1), tf.argmax(y, 1))
		accuracy = tf.reduce_mean(tf.cast(correct, 'float'))
		print('Accuracy: ' + accuracy.eval({x: 'mnist.test.images', y:'mnist.test.labels'}))


env.reset(train_mode=train_mode, config=None)

env.close()


