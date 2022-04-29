package net.wojtek.zti.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonManagedReference;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

@NoArgsConstructor
@Getter @Setter
@Entity
@Table(name = "user")
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private int id;

    @Column(name = "username")
    private String username;

    @Column(name = "password")
    @JsonIgnore
    private String password;

    @OneToMany(mappedBy = "sender")
    @JsonIgnore
//    @JsonIgnore
    private List<Message> messagesSent = new ArrayList<>();

    @OneToMany(mappedBy = "receiver")
    @JsonIgnore
//    @JsonIgnore
    private List<Message> messagesReceived = new ArrayList<>();

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        User user = (User) o;
        return id == user.id && Objects.equals(username, user.username) && Objects.equals(password, user.password) && Objects.equals(messagesSent, user.messagesSent) && Objects.equals(messagesReceived, user.messagesReceived);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, username, password, messagesSent, messagesReceived);
    }

}
