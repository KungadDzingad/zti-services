package net.wojtek.zti.services;

import net.wojtek.zti.dao.MessageRepository;
import net.wojtek.zti.models.Message;
import net.wojtek.zti.models.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@Service
public class MessageService {

    @Autowired
    private MessageRepository messageRepo;

    public void createMessage(User sender, User receiver, String content) {
        Message message = new Message();
        message.setContent(content);
        message.setSender(sender);
        message.setReceiver(receiver);
        message.setDate(LocalDateTime.now());
        messageRepo.save(message);
    }

    public List<Message> filterMessagesByReceiver(List<Message> messages, User userById) {
        return messages.stream().filter(message -> message.getReceiver().getId() == userById.getId())
                .collect(Collectors.toList());
    }

    public List<Message> getConversationMessages(User sender, User receiver) {
        List<Message> senderMessages = sender.getMessagesSent().stream()
                .filter(message -> message.getReceiver().equals(receiver))
                .collect(Collectors.toList());
        List<Message> receiverMessages = sender.getMessagesReceived().stream()
                .filter(message -> message.getSender().equals(receiver))
                .collect(Collectors.toList());
        senderMessages.addAll(receiverMessages);
        return senderMessages.stream()
                .sorted(Comparator.comparing(Message::getDate))
                .collect(Collectors.toList());
    }
}
